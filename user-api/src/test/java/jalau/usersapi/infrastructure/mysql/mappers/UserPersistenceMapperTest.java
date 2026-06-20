package jalau.usersapi.infrastructure.mysql.mappers;

import jalau.usersapi.core.domain.entities.User;
import jalau.usersapi.infrastructure.mysql.entities.UserJpaEntity;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.extension.ExtendWith;
import org.mockito.InjectMocks;
import org.mockito.junit.jupiter.MockitoExtension;

import static org.junit.jupiter.api.Assertions.assertEquals;

import static org.junit.jupiter.api.Assertions.assertNull;

/**
 * Unit tests for {@link UserPersistenceMapper}.
 * <p>
 * Ensures correct conversion between domain and persistence entities.
 */
@ExtendWith(MockitoExtension.class)
public class UserPersistenceMapperTest {
	
	/** Mapper under test */
	@InjectMocks
	private UserPersistenceMapper mapper;
	
	/**
	 * Should correctly map JPA entity to domain entity.
	 */
	@Test
	void shouldMapJpaToDomain() {
		UserJpaEntity entity = new UserJpaEntity();
		entity.setName("Javier");
		
		User user = mapper.toDomainEntity(entity);
		assertEquals("Javier", user.getName());
	}
	
	/**
	 * Should correctly map domain entity to JPA entity.
	 */
	@Test
	void shouldMapDomainToJpa() {
		User user = new User();
		user.setName("Javier");
		user.setLogin("jroca");
		
		UserJpaEntity entity = mapper.toJpaEntity(user);
		assertEquals("Javier", entity.getName());
		assertEquals("jroca", entity.getLogin());
	}

	@Test
	void shouldReturnNullWhenMappingJpaToDomainWithNull() {
		assertNull(mapper.toDomainEntity(null));
	}

	@Test
	void shouldReturnNullWhenMappingDomainToJpaWithNull() {
		assertNull(mapper.toJpaEntity(null));
	}
}
