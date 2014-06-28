﻿using GridMvc.Columns;
using GridMvc.Html;
using MvcTemplate.Components.Security;
using MvcTemplate.Resources;
using System;
using System.Globalization;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace MvcTemplate.Components.Extensions.Html
{
    public static class GridMvcExtensions
    {
        private static String CurrentAccountId
        {
            get
            {
                return HttpContext.Current.User.Identity.Name;
            }
        }
        private static String CurrentArea
        {
            get
            {
                return HttpContext.Current.Request.RequestContext.RouteData.Values["area"] as String;
            }
        }
        private static String CurrentController
        {
            get
            {
                return HttpContext.Current.Request.RequestContext.RouteData.Values["controller"] as String;
            }
        }

        public static IGridColumn<T> AddActionLink<T>(this IGridColumnCollection<T> column, LinkAction action) where T : ILinkableView
        {
            if (RoleFactory.Provider != null && !RoleFactory.Provider.IsAuthorizedFor(CurrentAccountId, CurrentArea, CurrentController, action.ToString()))
                return null;

            IGridColumn<T> gridColumn = column
                .Add()
                .SetWidth(25)
                .Encoded(false)
                .Sanitized(false)
                .Css("action-link-cell");

            AddLinkHtml(gridColumn, action);
            return gridColumn;
        }
        public static IGridColumn<T> AddDateProperty<T>(this IGridColumnCollection<T> column, Expression<Func<T, DateTime?>> property)
        {
            return column
                .AddDateTimeProperty(property)
                .Format(String.Format("{{0:{0}}}", CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern));
        }
        public static IGridColumn<T> AddDateTimeProperty<T>(this IGridColumnCollection<T> column, Expression<Func<T, DateTime?>> property)
        {
            return column
                .AddProperty(property)
                .Css("date-cell");
        }
        public static IGridColumn<T> AddProperty<T, TKey>(this IGridColumnCollection<T> column, Expression<Func<T, TKey>> property)
        {
            return column
                .Add(property)
                .Titled(ResourceProvider.GetPropertyTitle(property));
        }

        public static IGridHtmlOptions<T> ApplyAttributes<T>(this IGridHtmlOptions<T> options) where T : class
        {
            return options
                .EmptyText(Resources.Table.Resources.NoDataFound)
                .SetLanguage(CultureInfo.CurrentCulture.Name)
                .Named(typeof(T).Name)
                .WithMultipleFilters()
                .WithPaging(15)
                .Filterable()
                .Sortable();
        }

        private static void AddLinkHtml<T>(IGridColumn<T> gridColumn, LinkAction action) where T : ILinkableView
        {
            switch (action)
            {
                case LinkAction.Details:
                    gridColumn.RenderValueAs(GetDetailsLink);
                    break;
                case LinkAction.Edit:
                    gridColumn.RenderValueAs(GetEditLink);
                    break;
                case LinkAction.Delete:
                    gridColumn.RenderValueAs(GetDeleteLink);
                    break;
            }
        }
        private static String GetDetailsLink<T>(T model) where T : ILinkableView
        {
            return GetLink(model, LinkAction.Details, "fa fa-info");
        }
        private static String GetEditLink<T>(T model) where T : ILinkableView
        {
            return GetLink(model, LinkAction.Edit, "fa fa-pencil");
        }
        private static String GetDeleteLink<T>(T model) where T : ILinkableView
        {
            return GetLink(model, LinkAction.Delete, "fa fa-times");
        }
        private static String GetLink<T>(T model, LinkAction action, String iconClass) where T : ILinkableView
        {
            TagBuilder actionContainer = new TagBuilder("div");
            TagBuilder actionTag = new TagBuilder("a");
            TagBuilder icon = new TagBuilder("i");

            actionContainer.AddCssClass(String.Format("action-link-container {0}-action-link", action.ToString().ToLower()));
            actionTag.MergeAttribute("href", new UrlHelper(HttpContext.Current.Request.RequestContext).Action(action.ToString(), new { id = model.Id }));
            icon.AddCssClass(iconClass);

            actionTag.InnerHtml = icon.ToString();
            actionContainer.InnerHtml = actionTag.ToString();

            return actionContainer.ToString();
        }
    }
}
