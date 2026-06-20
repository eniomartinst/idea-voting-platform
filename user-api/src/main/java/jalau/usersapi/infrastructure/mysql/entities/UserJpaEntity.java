package jalau.usersapi.infrastructure.mysql.entities;

import lombok.Data;

/**
 * JPA entity representing a user in the MySQL database.
 */
@Data
public class UserJpaEntity {
	private String id;
	private String name;
	private String login;
	private String password;
}
