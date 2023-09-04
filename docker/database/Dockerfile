# Use the official PostgreSQL image from Docker Hub
FROM postgres:latest

# Set environment variables for PostgreSQL
ENV POSTGRES_USER=pguser
ENV POSTGRES_PASSWORD=pGL0cal
ENV POSTGRES_DB=quartznet
ENV PGDATA=/var/lib/postgresql/data/pgdata
# Expose the PostgreSQL port
EXPOSE 5432

# Copy your SQL initialization script into the container
COPY tables_postgres.sql /docker-entrypoint-initdb.d/

# Start the PostgreSQL server when the container is launched
CMD ["postgres"]
