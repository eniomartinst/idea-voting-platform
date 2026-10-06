package com.ideavoting.usersapi.models;

import org.junit.jupiter.api.Test;

import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.junit.jupiter.api.Assertions.assertNull;

class UserMapperTest {
	
	private final UserMapper mapper = new UserMapper();
	
	@Test
	void shouldMapDomainToResponseDto() {
		User user = User.builder().id("1").name("Javier").login("jroca").password("123").build();
		
		UserResponseDto dto = mapper.toResponseDtoEntity(user);
		
		assertEquals(user.getName(), dto.getName());
	}
	
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
		UserUpdateDto dto = new UserUpdateDto();
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