﻿namespace Telerik.Web.Mvc.UI.Tests
{
    using System;
    using System.Web.Mvc;
    using Xunit;
    using Telerik.Web.Mvc.UI.Fluent;

    public class DatePickerClientEventsBuilderTests
    {
        private DatePickerClientEventsBuilder builder;
        private DatePickerClientEvents clientEvents;
        private ViewContext viewContext;


        public DatePickerClientEventsBuilderTests()
        {
            clientEvents = new DatePickerClientEvents();
            viewContext = new ViewContext();
            builder = new DatePickerClientEventsBuilder(clientEvents, viewContext);
        }

        [Fact]
        public void OnDateChanged_method_with_Action_param_should_set_OnSelect_property()
        {
            Action param = () => { };

            builder.OnChange(param);

            Assert.NotNull(clientEvents.OnChange.InlineCode);
        }

        [Fact]
        public void OnDateChanged_method_with_string_param_should_set_OnSelect_property()
        {
            const string param = "my_method()";

            builder.OnChange(param);

            Assert.NotNull(clientEvents.OnChange.HandlerName);
        }

        [Fact]
        public void OnDateChanged_method_with_Action_param_should_return_builder()
        {
            Action param = () => { };

            var returned = builder.OnChange(param);

            Assert.IsType(typeof(DatePickerClientEventsBuilder), returned);
        }

        [Fact]
        public void OnDateChanged_method_with_string_param_should_return_builder()
        {
            const string param = "my_method()";

            var returned = builder.OnChange(param);

            Assert.IsType(typeof(DatePickerClientEventsBuilder), returned);
        }

        [Fact]
        public void Loaded_with_Action_param_should_set_Loaded_property()
        {
            Action param = () => { };

            builder.OnLoad(param);

            Assert.NotNull(clientEvents.OnLoad.InlineCode);
        }

        [Fact]
        public void Loaded_with_String_param_should_set_Loaded_property()
        {
            const string param = "my_method()";

            builder.OnLoad(param);

            Assert.NotNull(clientEvents.OnLoad.HandlerName);
        }

        [Fact]
        public void Loaded_with_Action_should_return_builder()
        {
            Action param = () => { };

            var returned = builder.OnLoad(param);

            Assert.IsType(typeof(DatePickerClientEventsBuilder), returned);
        }

        [Fact]
        public void Loaded_with_String_should_return_builder()
        {
            const string param = "my_method()";

            var returned = builder.OnLoad(param);

            Assert.IsType(typeof(DatePickerClientEventsBuilder), returned);
        }

        [Fact]
        public void OnPopupOpen_with_Action_param_should_set_Loaded_property()
        {
            Action param = () => { };

            builder.OnOpen(param);

            Assert.NotNull(clientEvents.OnOpen.InlineCode);
        }

        [Fact]
        public void OnPopupOpen_with_String_param_should_set_Loaded_property()
        {
            const string param = "my_method()";

            builder.OnOpen(param);

            Assert.NotNull(clientEvents.OnOpen.HandlerName);
        }

        [Fact]
        public void OnPopupOpen_with_Action_should_return_builder()
        {
            Action param = () => { };

            var returned = builder.OnOpen(param);

            Assert.IsType(typeof(DatePickerClientEventsBuilder), returned);
        }

        [Fact]
        public void OnPopupOpen_with_String_should_return_builder()
        {
            const string param = "my_method()";

            var returned = builder.OnOpen(param);

            Assert.IsType(typeof(DatePickerClientEventsBuilder), returned);
        }

        [Fact]
        public void OnPopupClose_with_Action_param_should_set_Loaded_property()
        {
            Action param = () => { };

            builder.OnClose(param);

            Assert.NotNull(clientEvents.OnClose.InlineCode);
        }

        [Fact]
        public void OnPopupClose_with_String_param_should_set_Loaded_property()
        {
            const string param = "my_method()";

            builder.OnClose(param);

            Assert.NotNull(clientEvents.OnClose.HandlerName);
        }

        [Fact]
        public void OnPopupClose_with_Action_should_return_builder()
        {
            Action param = () => { };

            var returned = builder.OnClose(param);

            Assert.IsType(typeof(DatePickerClientEventsBuilder), returned);
        }

        [Fact]
        public void OnPopupClose_with_String_should_return_builder()
        {
            const string param = "my_method()";

            var returned = builder.OnClose(param);

            Assert.IsType(typeof(DatePickerClientEventsBuilder), returned);
        }
    }
}