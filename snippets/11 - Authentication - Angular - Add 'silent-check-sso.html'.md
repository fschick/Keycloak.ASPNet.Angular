Create file `public\silent-check-sso.html` with content

```html
<html>
<body>
<script>
  parent.postMessage(location.href, location.origin)
</script>
</body>
</html>
```