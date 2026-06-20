package jalau.usersapi.infrastructure.mysql.mybatis;

import jalau.usersapi.infrastructure.mysql.entities.UserJpaEntity;
import org.apache.ibatis.annotations.Param;

import java.util.List;

/**
 * MyBatis mapper interface for executing SQL queries on the users table.
 */
public interface UserMyBatisMapper {
	/**
	 * Retrieves all users from the database.
	 *
	 * @return a list of UserJpaEntity objects
	 */
	List<UserJpaEntity> findAllUsers();
	
	/**
	 * Inserts a new user into the database.
	 *
	 * @param user the user entity to be persisted
	 */
	void createUser(UserJpaEntity user);
	
	void updateUser(UserJpaEntity user);

	UserJpaEntity getUserById(@Param("id") String id);
	
	void deleteUserById(@Param("id") String id);

	UserJpaEntity getUserByLogin(@Param("login") String login);
}
