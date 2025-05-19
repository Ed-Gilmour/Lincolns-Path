For final build
- Put the page background in TemplateData
- Remove Unity logo and game name from html
- Change favicon in html from unity logo to the lincoln icon
- Paste this before the closing header in index.html to add background
<style>
    body {
    margin: 0;
    padding: 0;
    background: url('TemplateData/PageBackground.png') no-repeat center center fixed;
    background-size: cover;
    }
</style>