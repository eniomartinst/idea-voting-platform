package jalau.usersapi;

import org.mybatis.spring.annotation.MapperScan;
import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;

/**
 * Main entry point for the Users API Spring Boot application.
 * Configures component scanning and MyBatis mapper scanning.
 */
@SpringBootApplication
@MapperScan("jalau.usersapi.infrastructure.mysql.mybatis")
public class UsersApiApplication {
	
	/**
	 * Starts the Spring Boot application.
	 *
	 * @param args application arguments
	 */
	public static void main(String[] args) {
		SpringApplication.run(UsersApiApplication.class, args);
	}
}
