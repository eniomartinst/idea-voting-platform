package jalau.usersapi.infrastructure.mysql.repositories;

import jalau.usersapi.core.domain.entities.User;
import jalau.usersapi.core.domain.repositories.IUserRepository;
import jalau.usersapi.infrastructure.mysql.entities.UserJpaEntity;
import jalau.usersapi.infrastructure.mysql.mappers.UserPersistenceMapper;
import jalau.usersapi.infrastructure.mysql.mybatis.UserMyBatisMapper;
import lombok.RequiredArgsConstructor;
import org.springframework.stereotype.Repository;
import org.springframework.context.annotation.Profile;

import java.util.List;

/**
 * Repository implementation for accessing users in the MySQL database.
 */
@Repository
@Profile("mysql")
@RequiredArgsConstructor
public class UserRepository implements IUserRepository {
	
	private final UserMyBatisMapper myBatisMapper;
	private final UserPersistenceMapper mapper;
	
	/**
	 * Retrieves all users from the database.
	 *
	 * @return a list of domain User entities
	 */
	@Override
	public List<User> getUsers() {
		List<UserJpaEntity> entities = myBatisMapper.findAllUsers();
		return entities.stream().map(mapper::toDomainEntity).toList();
	}
	
	@Override
	public User getUser(String id) {
		UserJpaEntity entity = myBatisMapper.getUserById(id);
		return entity != null ? mapper.toDomainEntity(entity) : null;
	}
	
	/**
	 * Creates a new user in the database.
	 * <p>
	 * The domain entity is converted to a persistence entity before being
	 * inserted into the database. The same entity is then mapped back
	 * to the domain model.
	 *
	 * @param user the user to be created
	 * @return the created user
	 */
	@Override
	public User createUser(User user) {
		UserJpaEntity entity = mapper.toJpaEntity(user);
		myBatisMapper.createUser(entity);
		return mapper.toDomainEntity(entity);
	}
	
	@Override
	public User updateUser(User user) {
		UserJpaEntity entity = mapper.toJpaEntity(user);
		myBatisMapper.updateUser(entity);
		return mapper.toDomainEntity(entity);
	}
	
	@Override
	public void deleteUser(String id) {
		myBatisMapper.deleteUserById(id);
	}

	@Override
	public User getUserByLogin(String login) {
		UserJpaEntity entity = myBatisMapper.getUserByLogin(login);
		return entity != null ? mapper.toDomainEntity(entity) : null;
	}
}
