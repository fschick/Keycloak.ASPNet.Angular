# Download Keycloak and dependencies

OpenJDK: https://learn.microsoft.com/en-us/java/openjdk/download

Keycloak: https://www.keycloak.org/downloads

# Keycloak (local)

## Create HTTPS certificate

```powershell
cd source
dotnet dev-certs https --trust --export-path certs\development.pem --no-password --format PEM
```

Add the following lines to `conf/keycloak.conf`  

```
cache=local
bootstrap-admin-username=tmp_admin
bootstrap-admin-password=admin
https-certificate-file=${kc.home.dir}/../certs/development.pem
https-certificate-key-file=${kc.home.dir}/../certs/development.key
hostname=localhost
```

```cmd
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
cd source
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
cd source
ng new frontend --minimal --style scss --ssr false
cd frontend
npm run start
```
