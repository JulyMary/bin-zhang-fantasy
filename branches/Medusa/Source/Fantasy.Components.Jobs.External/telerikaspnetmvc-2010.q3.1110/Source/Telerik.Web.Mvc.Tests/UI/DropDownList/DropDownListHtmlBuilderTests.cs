﻿namespace Telerik.Web.Mvc.UI.Tests
{
    using Moq;
    using Xunit;

    using System.IO;
    using System.Web.UI;
using System.Collections.Generic;
    using Telerik.Web.Mvc.Infrastructure;

    public class DropDownListHtmlBuilderTests
    {

        private DropDownList dropDownList;
        private DropDownListHtmlBuilder renderer;

        public DropDownListHtmlBuilderTests()
        {
            dropDownList = DropDownListTestHelper.CreateDropDownList();
            renderer = new DropDownListHtmlBuilder(dropDownList);
            dropDownList.Name = "DropDownList1";
        }

        [Fact]
        public void Build_should_output_start_tag()
        {
            IHtmlNode tag = renderer.Build();

            Assert.Equal("div", tag.TagName);
        }

        [Fact]
        public void Build_should_output_render_id()
        {
            const string id = "testName";
            dropDownList.Name = id;

            IHtmlNode tag = renderer.Build();

            Assert.Equal(id, tag.Attribute("id"));
        }

        [Fact]
        public void Build_should_output_render_css_classes()
        {
            const string css = "t-widget t-dropdown t-header";
            
            IHtmlNode tag = renderer.Build();

            Assert.Equal(css, tag.Attribute("class"));
        }

        [Fact]
        public void Build_should_render_html_attributes()
        {
            dropDownList.HtmlAttributes.Add("title", "genericInput");

            IHtmlNode tag = renderer.Build();

            Assert.Equal("genericInput", tag.Attribute("title"));
        }

        [Fact]
        public void Build_should_call_InnerContentMethod_and_append_to_wrapper() 
        {
            IHtmlNode tag = renderer.Build().Children[0];
            
            Assert.Equal("div", tag.TagName);
            Assert.True(tag.Attribute("class").Contains("t-dropdown-wrap"));
        }

        [Fact]
        public void Build_should_call_HiddenInputTag_and_append_to_wrapper()
        {
            IHtmlNode tag = renderer.Build().Children[1];

            Assert.Equal("input", tag.TagName);
        }

        [Fact]
        public void DropDownListInnerContentTag_should_output_wrapping_span_tag()
        {
            IHtmlNode tag = renderer.InnerContentTag();

            Assert.Equal("div", tag.TagName);
        }

        [Fact]
        public void DropDownListInnerContentTag_should_output_link_with_css_class()
        {
            IHtmlNode tag = renderer.InnerContentTag();

            Assert.Equal("t-dropdown-wrap t-state-default", tag.Attribute("class"));
        }

        [Fact]
        public void DropDownListInnerContentTag_should_output_child_text_span_with_css_class()
        {
            IHtmlNode tag = renderer.InnerContentTag();

            IHtmlNode textSpan = tag.Children[0];

            Assert.Equal("t-input", textSpan.Attribute("class"));
        }

        [Fact]
        public void DropDownListInnerContentTag_should_output_second_child_buttonSpan()
        {
            IHtmlNode tag = renderer.InnerContentTag();

            IHtmlNode textSpan = tag.Children[1];

            Assert.Equal("span", textSpan.TagName);
        }

        [Fact]
        public void DropDownListInnerContentTag_should_contain_empty_value_if_no_items()
        {
            dropDownList.Items.Clear();

            IHtmlNode tag = renderer.InnerContentTag();

            IHtmlNode textSpan = tag.Children[0];

            Assert.Equal("&nbsp;", textSpan.InnerHtml);
        }

        [Fact]
        public void DropDownListInnerContentTag_should_contain_selected_item_text()
        {
            dropDownList.Items.Add(new DropDownItem { Text = "Item1", Value = "1" });
            dropDownList.Items.Add(new DropDownItem { Text = "Item2", Value = "2" });
            dropDownList.SelectedIndex = 0;

            IHtmlNode tag = renderer.InnerContentTag();

            IHtmlNode textSpan = tag.Children[0];

            Assert.Equal("Item1", textSpan.InnerHtml);
        }

        [Fact]
        public void DropDownListInnerContentTag_should_render_two_child_spans_in_correct_order() 
        {
            IHtmlNode tag = renderer.InnerContentTag();

            var firstChild = tag.Children[0];
            var secondChild = tag.Children[1];

            Assert.Equal("t-input", firstChild.Attribute("class"));
            Assert.Equal("t-select", secondChild.Attribute("class"));
        }

        [Fact]
        public void HiddenInputTag_should_render_input() 
        {
            IHtmlNode tag = renderer.HiddenInputTag();

            Assert.Equal("input", tag.TagName);
        }

        [Fact]
        public void HiddenInputTag_should_output_input_type_text_()
        {
            IHtmlNode tag = renderer.HiddenInputTag();

            Assert.Equal("text", tag.Attribute("type"));
        }

        [Fact]
        public void HiddenInputTag_should_output_input_which_is_hidden_with_style()
        {
            IHtmlNode tag = renderer.HiddenInputTag();

            Assert.Equal("display:none", tag.Attribute("style"));
        }

        [Fact]
        public void HiddenInputTag_should_output_input_with_name()
        {
            dropDownList.Name = "test";

            IHtmlNode tag = renderer.HiddenInputTag();

            Assert.Equal(dropDownList.Id + "-value", tag.Attribute("id"));
            Assert.Equal(dropDownList.Name, tag.Attribute("name"));
        }

        [Fact]
        public void HiddenInputTag_should_not_add_attr_value_if_no_items() 
        {
            dropDownList.Items.Clear();

            IHtmlNode tag = renderer.HiddenInputTag();
            Assert.Throws(typeof(System.Collections.Generic.KeyNotFoundException), () => tag.Attribute("value"));
        }

        [Fact]
        public void HiddenInputTag_should_not_add_attr_value_with_selected_item_value()
        {
            dropDownList.Items.Add(new DropDownItem { Text = "Item1", Value = "1" });
            dropDownList.Items.Add(new DropDownItem { Text = "Item2", Value = "2" });
            dropDownList.SelectedIndex = 0;

            IHtmlNode tag = renderer.HiddenInputTag();
            Assert.Equal("1", tag.Attribute("value"));
        }


        [Fact]
        public void HiddenInputTag_should_add_attr_value_with_selected_item_text_if_value_is_not_set()
        {
            dropDownList.Items.Add(new DropDownItem { Text = "Item1", Value = "1" });
            dropDownList.Items.Add(new DropDownItem { Text = "Item2" });

            dropDownList.SelectedIndex = 1;

            IHtmlNode tag = renderer.HiddenInputTag();

            Assert.Equal("Item2", tag.Attribute("value"));
        }

        [Fact]
        public void HiddenInputTag_does_not_output_name_attribute_for_unnamed_components()
        {
            var renderer = new DropDownListHtmlBuilder(new EditorDropDown("FontFace", new List<DropDownItem>() { new DropDownItem { Text = "Arial", Value = "Arial,Verdana,sans-serif" } } ));

            IHtmlNode tag = renderer.HiddenInputTag();

            Assert.False(tag.Attributes().ContainsKey("name"));
        }

        [Fact]
        public void ComboBox_should_should_be_disabled()
        {
            dropDownList.Enabled = false;

            IHtmlNode div = renderer.Build();

            Assert.Equal("disabled", div.Attribute("disabled"));
            Assert.Contains("t-state-disabled", div.Attribute("class"));
        }
    }
}