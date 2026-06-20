package jalau.usersapi.presentation.mappers;

import jalau.usersapi.core.domain.entities.User;
import jalau.usersapi.presentation.dtos.UserCreateDto;
import jalau.usersapi.presentation.dtos.UserResponseDto;
import org.junit.jupiter.api.Test;

import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.junit.jupiter.api.Assertions.assertNull;

/**
 * Unit tests for {@link UserMapper}.
 * <p>
 * Ensures correct mapping between DTOs and domain entities.
 */
class UserMapperTest {
	
	/** Mapper under test */
	private final UserMapper mapper = new UserMapper();
	
	/**
	 * Should correctly map User to UserResponseDto.
	 */
	@Test
	void shouldMapDomainToResponseDto() {
		User user = new User("1", "Javier", "jroca", "123");
		
		UserResponseDto dto = mapper.toResponseDtoEntity(user);
		
		assertEquals(user.getName(), dto.getName());
	}
	
	/**
	 * Should correctly map UserCreateDto to User
	 */
	@Test
	void shouldMapDtoToDomain() {
		UserCreateDto dto = new UserCreateDto();
		dto.setName("Javier");
		dto.setLogin("jroca");
		dto.setPassword("pass123");
		
		User user = mapper.toDomainEntity(dto);
		
		assertEquals("Javier", user.getName());
		assertEquals("jroca", user.getLogin());
	}
	
	@Test
	void shouldMapUpdateDtoToDomain() {
		jalau.usersapi.presentation.dtos.UserUpdateDto dto = new jalau.usersapi.presentation.dtos.UserUpdateDto();
		dto.setName("New Name");
		dto.setLogin("new_login");
		dto.setPassword("pwd123");
		
		User user = mapper.toDomainEntity("idx", dto);
		
		assertEquals("idx", user.getId());
		assertEquals("New Name", user.getName());
		assertEquals("new_login", user.getLogin());
		assertEquals("pwd123", user.getPassword());
	}

	@Test
	void shouldReturnNullWhenDomainIsNull() {
		assertNull(mapper.toResponseDtoEntity(null));
	}

	@Test
	void shouldReturnNullWhenMappingCreateDtoWithNull() {
		assertNull(mapper.toDomainEntity(null));
	}

	@Test
	void shouldReturnNullWhenMappingUpdateDtoWithNull() {
		assertNull(mapper.toDomainEntity("1", null));
	}
}
