// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMemberInSuper.Global

namespace Modulos
{
    public interface IOption<T> : IOptionBase
    {
        /// <summary>
        /// Friendly name of the option.
        /// </summary>
        string FriendlyName { get; }

        /// <summary>
        /// Current option value.
        /// </summary>
        T Value { get; set; }

        /// <summary>
        /// Runs once when application starts.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Returns default value of option. It's invoked by <see cref="Initialize"/> method.
        /// </summary>
        /// <returns>Default value of the option.</returns>
        T GetDefaultValue();
    }
}