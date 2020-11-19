﻿using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.Extensions.DependencyInjection;

namespace Modulos.Pipes
{
    public class Pipeline : IPipeline
    {
        private static readonly ConcurrentDictionary<Type, ObjectActivator> activators = new ConcurrentDictionary<Type, ObjectActivator>();

        private readonly IServiceProvider serviceProvider;
        private readonly List<Type> pipes = new List<Type>();

        public Pipeline(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }


        public void Add<T>() where T : IPipe
        {
            Add(typeof(T));
        }

        public void Add(Type pipeType)
        {
            ThrowIfWrongPipeType(pipeType);
            ThrowIfAlreadyExists(pipeType);
            pipes.Add(pipeType);
        }

        public bool Remove<T>() where T : IPipe
        {
            return Remove(typeof(T));
        }

        public bool Remove(Type pipeType) 
        {
            ThrowIfWrongPipeType(pipeType);
            return pipes.Remove(pipeType);
        }

        public void Insert<TPipeToFind, TPipeToInsert>(InsertType insertType)
            where TPipeToFind : IPipe
            where TPipeToInsert : IPipe
        {
            Insert(insertType,typeof(TPipeToFind),typeof(TPipeToInsert));
        }
        
        public void Insert(InsertType insertType, Type pipeToFind, Type pipeToInsert)
        {
            ThrowIfWrongPipeType(pipeToFind);
            ThrowIfWrongPipeType(pipeToInsert);

            ThrowIfAlreadyExists(pipeToInsert);
            var indexOf = pipes.IndexOf(pipeToFind);
            if(indexOf < 0)
                throw new ArgumentException($"Pipe does not exists: {pipeToFind.Name}.");
            switch (insertType)
            {
                case InsertType.After:
                    pipes.Insert(indexOf + 1, pipeToInsert);
                    break;
                case InsertType.Before:
                    pipes.Insert(indexOf, pipeToInsert);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(insertType), insertType, null);
            }

        }

        public void AddOrReplace<T>() where T : IPipe
        {
           AddOrReplace(typeof(T));
        } 

        public void AddOrReplace(Type pipeType)
        {
            ThrowIfWrongPipeType(pipeType);

            var index = pipes.IndexOf(pipeType);
            if (index < 0)
            {
                Add(pipeType);
                return;
            }

            pipes.Insert(index + 1, pipeType);
            pipes.RemoveAt(index);
        } 

        public void TryRemoveAndAdd<T>() where T : IPipe
        {
            TryRemoveAndAdd(typeof(T));
        } 

        public void TryRemoveAndAdd(Type pipeType)
        {
            ThrowIfWrongPipeType(pipeType);

            var index = pipes.IndexOf(pipeType);
            if (index >= 0)
            {
                pipes.RemoveAt(index);
            }
            Add(pipeType);
        }

        public int IndexOf<T>() where T : IPipe
        {
            return IndexOf(typeof(T));
        }

        public int IndexOf(Type pipeType)
        {
            ThrowIfWrongPipeType(pipeType);

            return pipes.IndexOf(pipeType);
        }



        public IEnumerable<Type> GetPipes()
        {
            return pipes.AsReadOnly();
        }

        public IEnumerator<Type> GetEnumerator()
        {
            return pipes.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public async Task<IPipelineResult> Execute(CancellationToken cancellationToken, params object[] additionalReferences)
        {
            var optionalPipes = new List<Type>();
            var references = new List<object>(additionalReferences);
            //var publishedObjects = new List<object>();

            foreach (var pipeType in pipes)
            {
                var breakBecauseOptional = false;
                foreach (var optionalPipe in optionalPipes)
                {
                    var resolveOptionalResult = ResolvePipe(optionalPipe, references.ToArray());
                    if (!resolveOptionalResult.Success)
                        continue;

                    var optional = resolveOptionalResult.Pipe;
                    references.Add(optional);
                    //publishedObjects.Add(optional);

                    var optionalResult = await optional.Execute(cancellationToken);
                    references.AddRange(optionalResult.PublishedData);
                    //publishedObjects.AddRange(optionalResult.PublishedData);

                    switch (optionalResult.Action)
                    {
                        case PipeActionAfterExecute.Continue:
                            continue;
                        case PipeActionAfterExecute.Break:
                            breakBecauseOptional = true;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }

                if (breakBecauseOptional)
                    break;

                var resolveResult = ResolvePipe(pipeType, references.ToArray());
               
                if (!resolveResult.Success)
                {
                    optionalPipes.Add(pipeType);
                    continue;
                }

                var instance = resolveResult.Pipe;
                references.Add(instance);
                //publishedObjects.Add(instance);
                var result = await instance.Execute(cancellationToken);

                references.AddRange(result.PublishedData);
                //publishedObjects.AddRange(result.PublishedData);
               
                switch (result.Action)
                {
                    case PipeActionAfterExecute.Continue:
                        continue;
                    case PipeActionAfterExecute.Break:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                return new PipelineResult(references);
            }

            return new PipelineResult(references);
        }

        private class ResolvePipeResult
        {
            public bool Success { get; }
            public IPipe Pipe { get; }

            public ResolvePipeResult(bool success, IPipe pipe)
            {
                Success = success;
                Pipe = pipe;
            }
        }

        private ResolvePipeResult ResolvePipe(Type typeToResolve,  params object[] additionalData)
        {
            var ctor = typeToResolve.GetConstructors()
                .Select(e => (ctor: e, count: e.GetParameters().Length))
                .OrderByDescending(e => e.count)
                .Select(e => e.ctor)
                .First();

            
            var @params = additionalData.ToDictionary(e => e.GetType(), e => e);

            var parameters = new List<object>();
            foreach (var paramInfo in ctor.GetParameters())
            {
                var keyFromAdditional = @params.Keys
                    .LastOrDefault(e => paramInfo.ParameterType.IsAssignableFrom(e));

                if (keyFromAdditional != null)
                {
                    var paramFromAdditional = @params[keyFromAdditional];

                    if (paramFromAdditional != null)
                    {
                        parameters.Add(paramFromAdditional);
                        continue;
                    }
                }

                if ((paramInfo.Attributes & ParameterAttributes.Optional) != 0)
                {
                    var value = serviceProvider.GetService(paramInfo.ParameterType);
                    parameters.Add(value);
                }
                else
                {
                    try
                    {
                        var value = serviceProvider.GetRequiredService(paramInfo.ParameterType);
                        parameters.Add(value);
                    }
                    catch (Exception e)
                    {
                        if (typeof(IOptionalPipe).IsAssignableFrom(typeToResolve))
                            return new ResolvePipeResult(false, null);

                        throw new TodoException
                        (
                            $"Unable to resolve pipe: {typeToResolve.FullName} " +
                            $"parameter: {paramInfo.Name} of type {paramInfo.ParameterType.FullName}.", e
                        );
                    }
                }
            }

            if (!activators.TryGetValue(typeToResolve, out var activator))
            {
                activator = CreateObjectActivator(ctor);
                activators.TryAdd(typeToResolve, CreateObjectActivator(ctor));
            }

            var pipe = (IPipe) activator(parameters.ToArray());
            return new ResolvePipeResult(true,pipe);
        }

        private void ThrowIfAlreadyExists(Type pipeType)
        {
            if (pipes.Contains(pipeType))
                throw new ArgumentException($"Pipe already exists.", pipeType.Name);
        }

        private void ThrowIfWrongPipeType(Type pipeType)
        {
            if (!typeof(IPipe).IsAssignableFrom(pipeType))
                throw new ArgumentException("Not supported pipe type.", pipeType.Name);
        }

        private static ObjectActivator CreateObjectActivator(ConstructorInfo ctor)
        {
            var paramsInfo = ctor.GetParameters();
            var argsExp = new Expression[paramsInfo.Length];
            var param = Expression.Parameter(typeof(object[]), "args");

            //pick each arg from the params array 
            //and create a typed expression of them
            for (var i = 0; i < paramsInfo.Length; i++)
            {
                Expression index = Expression.Constant(i);
                var paramType = paramsInfo[i].ParameterType;

                Expression paramAccessorExp = Expression.ArrayIndex(param, index);

                Expression paramCastExp = Expression.Convert(paramAccessorExp, paramType);

                argsExp[i] = paramCastExp;
            }

            // Make a NewExpression that calls the ctor with the args we just created
            var newExp = Expression.New(ctor, argsExp);

            // Create a lambda with the New expression as body and our param object[] as arg
            var lambda = Expression.Lambda(typeof(ObjectActivator), newExp, param);

            var compiled = (ObjectActivator) lambda.Compile();

            return compiled;
        }

        private delegate object ObjectActivator(params object[] args);
    }
}