<p>Solution created in Microsoft Visual Studio 2017 Community Edition 15.3.3</p>
<p>Note: if you receive an "error CS0012: The type 'IEnumerable' is defined in assembly that is not referenced...", please ensure that your Visual Studio 2017 is up to date.  When I ran this on a separate machine to test, I had to update Visual Studio (which  there to mitigate this error.</p>

<h3>Health Catalyst - Patient Search Assignment</h3>
<br />

<h5>Business Requirements</h5>
<ul>
    <li> The application accepts search input in a text box and then displays in a pleasing style a list of people where any part of their first or last name matches what was typed in the search box (displaying at least name, address, age, interests, and a picture).</li>
    <li> Solution should either seed data or provide a way to enter new users or both</li>
    <li> Simulate search being slow and have the UI gracefully handle the delay</li>
</ul>
<br />

<h5>Technical Requirements</h5>
<ul>
    <li> An ASP.NET MVC Application</li>
    <li> Use Ajax to respond to search request (no full page refresh) using JSON for both the request and the response</li>
    <li> Use Entity Framework Code First to talk to the database</li>
    <li> Unit Tests for appropriate parts of the application</li>
</ul>
<br />
