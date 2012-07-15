<%@ Page Title="ClientAPI tests" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">

    <h2>Client API Tests</h2>
    
	<% Html.Telerik().Menu()
            .Name("Menu")
            .Items(items =>
            {
                items.Add().Text("Item 1")
                    .Items(item =>
                    {
                        item.Add().Text("Child Item 1.1");
                        item.Add().Text("Child Item 1.2");
                        item.Add().Text("Child Item 1.3");
                        item.Add().Text("Child Item 1.4");
                    });
                items.Add().Text("Item 2")
                                  .ImageHtmlAttributes(new { alt = "testImage", height = "10px", width = "10px" })
                                  .ImageUrl(Url.Content("~/Content/Images/telerik.png"));

                items.Add().Text("Item 3")
                    .Items(item =>
                    {
                        item.Add().Text("Child Item 3.1");
                        item.Add().Text("Child Item 3.2");
                        item.Add().Text("Child Item 3.3");
                        item.Add().Text("Child Item 3.4");
                    });
                items.Add().Text("Item 4")
                    .Items(item =>
                    {
                        item.Add().Text("Child Item 4.1")
                            .Items(subitem =>
                            {
                                subitem.Add().Text("Grand Child Item 4.1.1");
                                subitem.Add().Text("Grand Child Item 4.1.2");
                            });

                        item.Add().Text("Child Item 4.2");
                    }).Enabled(false);
                items.Add().Text("Item 5")
                    .Items(item =>
                    {
                        item.Add().Text("Child Item 5.1");
                    });
                items.Add().Text("Item 6")
                    .Items(item =>
                    {
                        item.Add().Text("Child Item 6.1");
                    });
                items.Add().Text("Item7");
                items.Add().Text("Item8").Enabled(false);
            }
            ).ClientEvents(events =>
            {
                events.OnOpen("Open");
                events.OnClose("Close");
                events.OnSelect("Select");
                events.OnLoad("Load");
            })
            .Effects(effects=> effects.Toggle())
            .Render(); %>
    
   <script type="text/javascript">

       var isRaised;
        
        function getRootItem(index) {
			return $('#Menu').find('> .t-item').eq(index)
        }

        function getMenu() {
            return $("#Menu").data("tMenu");
        }

        function test_click_method_should_call_preventDefault_method() {
            var item = getRootItem(7);
            var isCalled = false;

            var e = { preventDefault: function () { isCalled = true; }, stopPropagation: function () { } }

            getMenu().click(e, item);

            assertTrue(isCalled);
        }

        function test_click_method_should_call_stopPropagation_method_always() {
            var item = getRootItem(7);
            var isCalled = false;

            var e = { stopPropagation: function () { isCalled = true; }, preventDefault: function () { } }

            getMenu().click(e, item);

            assertTrue(isCalled);
        }

        function test_clicking_should_raise_onSelect_event() {
            var item = getRootItem(6);

            isRaised = false;

            item.find('> .t-link').trigger('click');

            assertTrue(isRaised);
        }

        function test_open_should_not_open_item_is_disabled() {
            var menu = getMenu();

            var item = getRootItem(3);

            menu.open(item);

            assertEquals("none", item.find('> .t-group').css("display"));
        }      

        function test_disable_method_should_disable_enabled_item() {
            var menu = getMenu();

            var item = getRootItem(2);

            menu.disable(item);
            
            assertTrue(item.hasClass('t-state-disabled'));
        }

        function test_enable_method_should_enable_disabled_item() {
            var menu = getMenu();

            var item = getRootItem(3);

            menu.enable(item);

            assertTrue(item.hasClass('t-state-default'));
        }

        function test_client_object_is_available_in_on_load() {
            assertNotNull(onLoadMenu);
            assertNotUndefined(onLoadMenu);
        }

        //handlers
        function Open() {
            isRaised = true;
        }

        function Close() {
            isRaised = true;
        }

        function Select() {
            isRaised = true;
        }


        var onLoadMenu;

        function Load() {
            isRaised = true;
            onLoadMenu = $(this).data('tMenu');
        }
   </script>

</asp:Content>