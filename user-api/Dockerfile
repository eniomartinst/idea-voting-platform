# Multi-stage build separates compilation (Maven) from execution (Java)
# This reduces the final image size

# Build stage - compiles the project
# Image with Maven + Java 21
FROM maven:3.9.9-eclipse-temurin-21 AS build

WORKDIR /app
# Copies all project files into the container
COPY . .
# Builds the application JAR (skips tests)
RUN mvn clean package -DskipTests

# Run stage - executes the application
# Lightweight image with only Java 21
FROM eclipse-temurin:21-jdk

WORKDIR /app
# Copies the generated JAR from the build stage
COPY --from=build /app/target/*.jar app.jar

# Exposes the application port
EXPOSE 8001

# Command to run the application
ENTRYPOINT ["java", "-jar", "app.jar"]
