package jalau.usersapi.presentation.controllers;

import jalau.usersapi.core.domain.services.IUserQueryService;
import jalau.usersapi.presentation.dtos.UserResponseDto;
import jalau.usersapi.presentation.mappers.UserMapper;
import lombok.RequiredArgsConstructor;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.CrossOrigin;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RestController;
import org.springframework.web.bind.annotation.RequestMapping;

import java.util.List;
import java.util.concurrent.CompletableFuture;

/**
 * REST controller for user queries.
 * Provides endpoints for retrieving user data.
 */
@CrossOrigin(origins = "*")
@RestController
@RequestMapping("/api/v1/users")
@RequiredArgsConstructor
public class UserQueryController {
	
	private final IUserQueryService userQueryService;
	private final UserMapper userMapper;
	
	/**
	 * GET /api/v1/users
	 * Retrieves all users and converts them to response DTOs asynchronously.
	 *
	 * @return a CompletableFuture containing ResponseEntity with list of UserResponseDto
	 */
	@GetMapping
	public CompletableFuture<ResponseEntity<List<UserResponseDto>>> getUsers() {
		return userQueryService.getUsers().thenApply(users -> {
			List<UserResponseDto> response = users.stream()
												 .map(userMapper::toResponseDtoEntity)
												 .toList();
			return ResponseEntity.ok(response);
		});
	}

	/**
	 * GET /api/v1/users/{id}
	 * Retrieves a single user by ID asynchronously.
	 *
	 * @param id the user ID
	 * @return a CompletableFuture containing ResponseEntity with the UserResponseDto
	 */
	@GetMapping("/{id}")
	public CompletableFuture<ResponseEntity<UserResponseDto>> getUser(@org.springframework.web.bind.annotation.PathVariable String id) {
		return userQueryService.getUser(id).thenApply(user -> {
			UserResponseDto response = userMapper.toResponseDtoEntity(user);
			return ResponseEntity.ok(response);
		});
	}
}
