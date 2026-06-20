package jalau.usersapi;

import org.junit.jupiter.api.Test;
import org.springframework.boot.test.context.SpringBootTest;
import org.springframework.test.context.ActiveProfiles;

/**
 * Basic integration test for the Spring Boot application context.
 * <p>
 * This test verifies that the application context loads successfully
 * without any configuration or dependency issues.
 */
@SpringBootTest
@ActiveProfiles("mongodb")
class UsersApiApplicationTests {
	
	/**
	 * Ensures that the Spring application context loads correctly.
	 */
	@Test
	void contextLoads() {
		// No implementation needed.
		// Test will fail if the application context cannot start.
	}
}
