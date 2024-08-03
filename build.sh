docker compose down
docker volume rm 560projectgroup2_postgres
docker build --file ./Dockerfile . -t sethbrittain/pidgin:1.0.0
docker compose up -d
