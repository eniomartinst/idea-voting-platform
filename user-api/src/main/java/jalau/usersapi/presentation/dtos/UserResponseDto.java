package jalau.usersapi.presentation.dtos;

import lombok.Data;

/**
 * Data Transfer Object used to return user information in API responses.
 */
@Data
public class UserResponseDto {
    
    /**
     * Unique identifier of the user.
     */
    private String id;
    
    /**
     * The user's full name.
     */
    private String name;
    
    /**
     * The user's login identifier.
     */
    private String login;
}
