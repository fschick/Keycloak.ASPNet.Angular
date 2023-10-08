Create file `Extensions\RptInterceptor.js` with content below and set property `Copy to Output Directory` to `Copy always`

```js
function entitlement(req, tokenUrl, resourceServerId) {
    const access_token = req.headers['Authorization'];

    const isAuthenticated = access_token !== undefined;
    if (!isAuthenticated)
        return req;

    const rptRequestHeadeers = {
        'Authorization': access_token,
        'Content-Type': 'application/x-www-form-urlencoded',
    };

    const rptRequestBody = new URLSearchParams({
        'grant_type': 'urn:ietf:params:oauth:grant-type:uma-ticket',
        'audience': resourceServerId,
    });
    
    const rptExecutor = async resolve => {
        const response = await fetch(tokenUrl, {method: 'POST', headers: rptRequestHeadeers, body: rptRequestBody});
        const responseBody = await response.json();
        req.headers['Authorization'] = `Bearer ${responseBody.access_token}`
        resolve(req);
    };

    return new Promise(rptExecutor);
}
```