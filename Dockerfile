# Stage 1 - Build React project
FROM node:18 as build-stage1

WORKDIR /client

COPY Frontend/package*.json ./

RUN npm install

COPY Frontend/ ./

RUN npm run build

# Stage 2 - Build ASP.NET project
FROM gradle:jdk17-graal-jammy as build-stage-2

WORKDIR /server

COPY Backend/ ./

COPY --from=0 /client/dist/ /server/src/main/resources/static

RUN gradle bootJar

# Stage 3 - Copy React build to wwwroot of asp project
FROM eclipse-temurin:17.0.8.1_1-jdk-ubi9-minimal as build-stage3

COPY --from=1 /server/build ./

# Https traffic
EXPOSE 8080

# Run Project
CMD ["java", "-jar", "/libs/DatabaseProject.jar"]