package jalau.usersapi.presentation.mappers;

import jalau.usersapi.core.domain.entities.User;
import jalau.usersapi.presentation.dtos.UserCreateDto;
import jalau.usersapi.presentation.dtos.UserResponseDto;
import jalau.usersapi.presentation.dtos.UserUpdateDto;
import org.springframework.stereotype.Component;

/**
 * Mapper class for converting domain User entities to response DTOs.
 */
@Component
public class UserMapper {
	
	/**
	 * Converts a domain User entity to a UserResponseDto.
	 *
	 * @param user the domain User
	 * @return the UserResponseDto, or null if input is null
	 */
	public UserResponseDto toResponseDtoEntity(User user) {
		if (user == null) return null;
		
		UserResponseDto dto = new UserResponseDto();
		dto.setId(user.getId());
		dto.setName(user.getName());
		dto.setLogin(user.getLogin());
		
		return dto;
	}
	
	/**
	 * Converts a UserCreateDto to a domain User entity.
	 *
	 * @param dto the request DTO
	 * @return the domain entity, or null if input is null
	 */
	public User toDomainEntity(UserCreateDto dto) {
		if (dto == null) return null;
		
		User user = new User();
		user.setName(dto.getName());
		user.setLogin(dto.getLogin());
		user.setPassword(dto.getPassword());
		
		return user;
	}
	
	// Converte UserUpdateDto para domínio User, usando o id
	public User toDomainEntity(String id, UserUpdateDto dto) {
		if (dto == null) return null;
		
		User user = new User();
		user.setId(id);
		user.setName(dto.getName());
		user.setLogin(dto.getLogin());
		user.setPassword(dto.getPassword());
		
		return user;
	}
}
