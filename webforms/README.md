# Web Forms Conversion

This folder contains the ASP.NET Web Forms version of the KUET Career Club project.

## What changed

- The static HTML site was converted into `.aspx` pages with a shared `Site.Master` layout.
- The contact and join flows now post back through Web Forms code-behind instead of the ASP.NET Core API.
- Submissions are stored in a local SQLite database at `webforms/App_Data/weblab.db`.

## Open in Visual Studio

Open `WebLab.WebForms.csproj` in Visual Studio with the ASP.NET and web development workload installed.

## Notes

- The original ASP.NET Core project in `backend/` was left untouched so the repository history stays intact.
- The Web Forms pages reuse the site-wide styling from the root `style.css` and `script.js` files.