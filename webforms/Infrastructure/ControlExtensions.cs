using System;
using System.Web.UI;

namespace WebLab.WebForms.Infrastructure
{
    public static class ControlExtensions
    {
        public static T FindDescendant<T>(this Control root, string id)
            where T : Control
        {
            foreach (Control child in root.Controls)
            {
                if (child.ID == id && child is T typedControl)
                {
                    return typedControl;
                }

                var nested = child.FindDescendant<T>(id);
                if (nested != null)
                {
                    return nested;
                }
            }

            return null;
        }

        public static T RequireDescendant<T>(this Control root, string id)
            where T : Control
        {
            return root.FindDescendant<T>(id) ?? throw new InvalidOperationException($"Unable to locate control '{id}'.");
        }
    }
}