Azure AD MVC Example
====================

Example of how to add Microsoft Azure Active Directory to an existing ASP.NET MVC 5 website.

See the blog series for [explanations](http://robdmoore.id.au/blog/2014/06/29/practical-microsoft-azure-active-directory-blog-series/).

For a detailed look at the step-by-step process that was followed to set up everything see the [commit list](https://github.com/robdmoore/AzureAdMvcExample/commits/master).

To visit the test site to see it in action check out [https://azureadmvcexample.azurewebsites.net](https://azureadmvcexample.azurewebsites.net).

![Screenshot](https://raw.githubusercontent.com/robdmoore/AzureAdMvcExample/master/screenshot.png)

Infrastructure Setup
--------------------

* Azure Active Directory tenant at `azureadmvcexample.onmicrosoft.com` with GUID `a83a168d-d1ad-4bde-9b0a-ed570162e695`
* Application with realm / audience URL `http://localhost:34999` and client ID `4e84d32e-7c1d-420b-88ac-e5cb655d292f` (matches the IIS Express setting for the local site)
* Application with realm / audience URL `https://azureadmvcexample.azurewebsites.net` and client ID `e4a511a6-f9a2-408c-a4a1-c687bc93e297`
* The following test users:
    * `username1@azureadmvcexample.onmicrosoft.com` with password `P@ssword2`, which is a member of `Group 1` and `Group 2`
	* `username2@azureadmvcexample.onmicrosoft.com` with password `P@ssword2`, which is a member of `Group 2` and `Group 3`
