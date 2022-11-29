#!/bin/sh

set -e

server -dev &

sleep 10

vault secrets enable database

vault write database/config/mssql-database \
    plugin_name=mssql-database-plugin \
    connection_url='sqlserver://{{username}}:{{password}}@sql-server:1433' \
    allowed_roles="lyft-client" \
    username="root" \
    password="P@ssw0rd"

vault write database/roles/lyft-client \
    db_name=mssql-database \
    creation_statements="CREATE LOGIN [{{name}}] WITH PASSWORD = '{{password}}';\
        CREATE USER [{{name}}] FOR LOGIN [{{name}}];\
        GRANT SELECT ON SCHEMA::dbo TO [{{name}}];" \
    default_ttl="1h" \
    max_ttl="24h"


vault secrets enable pki
vault secrets tune -max-lease-ttl=8760h pki
vault write pki/root/generate/internal \
    common_name=my-website.com \
    ttl=8760h

vault write pki/config/urls \
    issuing_certificates="http://127.0.0.1:8200/v1/pki/ca" \
    crl_distribution_points="http://127.0.0.1:8200/v1/pki/crl"

vault write pki/roles/lyft-client \
    allowed_domains=lyft-client;lyft-client.client \
    allow_subdomains=true \
    max_ttl=72h
