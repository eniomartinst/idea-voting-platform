package jalau.usersapi.infrastructure.mysql.mappers;

import jalau.usersapi.core.domain.entities.User;
import jalau.usersapi.infrastructure.mysql.entities.UserJpaEntity;
import org.springframework.stereotype.Component;

import java.util.UUID;

/**
 * Mapper class to convert database UserJpaEntity objects to domain User entities.
 */
@Component
public class UserPersistenceMapper {
	
	/**
	 * Maps a domain User entity to a database UserJpaEntity.
	 *
	 * @param user the domain entity
	 * @return the database entity, or null if input is null
	 */
	public UserJpaEntity toJpaEntity(User user) {
		if (user == null) return null;
		
		UserJpaEntity entity = new UserJpaEntity();
		
		// Generate UUID only for new users
		if (user.getId() == null || user.getId().isEmpty()) {
			entity.setId(UUID.randomUUID().toString());
		} else {
			entity.setId(user.getId());
		}
		
		entity.setName(user.getName());
		entity.setLogin(user.getLogin());
		entity.setPassword(user.getPassword());
		
		return entity;
	}
	
	/**
	 * Maps a UserJpaEntity to a domain User entity.
	 *
	 * @param entity the database entity
	 * @return the domain entity, or null if input is null
	 */
	public User toDomainEntity(UserJpaEntity entity) {
		if (entity == null) return null;
		
		User user = new User();
		user.setId(entity.getId());
		user.setName(entity.getName());
		user.setLogin(entity.getLogin());
		user.setPassword(entity.getPassword());
		
		return user;
	}
}
