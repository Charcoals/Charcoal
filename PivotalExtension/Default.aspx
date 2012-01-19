<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="PivotalExtension._Default" %>

<%@ Import Namespace="PivotalConnect" %>
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        Stories
    </h2>
    <p>
        <asp:CheckBox runat="server" ID="HideCompletedCheckbox" Checked="false" Text="Hide Completed Tasks"
            OnCheckedChanged="HideCompletedCheckbox_Click" AutoPostBack="true" />
    </p>
    <asp:Repeater ID="StoryRepeater" runat="server">
        <HeaderTemplate>
            <table class="mainTable">
        </HeaderTemplate>
        <ItemTemplate>
            <tr class="story">
                <td class="storyCol">
                    <div>
                        <%# ((Story)Container.DataItem).Name %>
                    </div>
                </td>
                <td>
                    <div>
                        <asp:Repeater ID="TaskRepeater" runat="server" DataSource="<%# ((Story)Container.DataItem).Tasks.Where(t=> !hideCompletedTasks || !t.Complete) %>">
                            <ItemTemplate>
                                <div class="<%# ((Task)Container.DataItem).GetStyle() %>">
                                    <%# ((Task)Container.DataItem).GetDescriptionWithoutOwners() %>
                                    <strong>
                                        <%# String.Join("/", ((Task)Container.DataItem).GetOwners().Select(o=>o.Initials)) %></strong>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </td>
            </tr>
        </ItemTemplate>
        <FooterTemplate>
            </table>
        </FooterTemplate>
    </asp:Repeater>
</asp:Content>
