# Keycloak (local)

```cmd
SET KC_BOOTSTRAP_ADMIN_USERNAME=tmp_admin
SET KC_BOOTSTRAP_ADMIN_PASSWORD=admin
kc start-dev
```

# Keycloak (docker)

```bash
docker volume create keycloak_data
# Copy any realm to import to /var/lib/docker/volumes/keycloak_data/_data/import or wherever the docker volumes are located
docker run -d --name keycloak --restart=always -p 8080:8080 -v keycloak_data:/opt/keycloak/data -e KC_BOOTSTRAP_ADMIN_USERNAME=tmp_admin -e KC_BOOTSTRAP_ADMIN_PASSWORD=admin quay.io/keycloak/keycloak start-dev --import-realm
```

# Back-End

```cmd
dotnet new sln --name backend --output backend
dotnet new webapi --auth None --no-https --no-openapi --use-program-main --use-controllers --name backend --output backend
cd backend
dotnet sln add .
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
dotnet add package Swashbuckle.AspNetCore
```

```c#
 builder.Services.AddSwaggerGen(options =>
 {
     options.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
 });

app.UseSwagger();

app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("v1/swagger.json", "My API V1");
});
```

# Front-End

```cmd
npm install -g @angular/cli
ng new frontend --minimal --style scss --ssr false
cd frontend
npm run start
```
