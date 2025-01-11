# Step 1: Use the official Grafana image
FROM grafana/grafana:latest

# Step 2: Set environment variables for Grafana
ENV GF_SECURITY_ADMIN_PASSWORD='admin'
ENV GF_AUTH_ANONYMOUS_ENABLED=true

# Step 3: Copy provisioning and dashboard files
COPY provisioning /etc/grafana/provisioning
COPY dashboards /etc/grafana/dashboards
#COPY grafana.ini /etc/grafana/grafana.ini

# Step 4: Expose the Grafana port
EXPOSE 3000

# Step 5: Set the entrypoint to run Grafana
ENTRYPOINT ["/bin/bash", "-c", "bin/grafana-server --homepath=/usr/share/grafana"]
