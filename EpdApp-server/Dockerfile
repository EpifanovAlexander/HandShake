FROM eclipse-temurin:18
ARG JAR_FILE=EPDApplication-0.0.2-SNAPSHOT.jar
COPY ${JAR_FILE} application.jar
ENTRYPOINT ["java", "-jar", "application.jar"]
