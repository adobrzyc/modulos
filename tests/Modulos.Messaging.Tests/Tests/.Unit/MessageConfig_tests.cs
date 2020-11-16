using System;
using System.Diagnostics.CodeAnalysis;
using Modulos.Messaging.Configuration;
using Xunit;

namespace Modulos.Messaging.Tests.Tests.Unit
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class MessageConfig_Tests
    {
        [Fact]
        public void check_if_Properties_property_is_readonly_after_freeze()
        {
            var config = new MessageConfig();

            config.Properties.Add("key1", "value1");
            config.Properties.Add("key2", "value2");
            config.Freeze();

            Assert.Throws<NotSupportedException>(() =>
            {
                config.Properties.Add("key3", "value3");
            });
           
        }

        [Fact]
        public void check_if_Properties_property_is_not_frozen_for_new_instance_based_on_frozen_one()
        {
            var config1 = new MessageConfig();
            config1.Properties.Add("key1", "value1");
            config1.Properties.Add("key2", "value2");
            config1.Freeze();
       
            Assert.Throws<NotSupportedException>(() =>
            {
                config1.Properties.Add("key3", "value3");
            });

            var config2 = new MessageConfig(config1);
            config2.Properties.Add("key3", "value3");

           
        }
    }
}