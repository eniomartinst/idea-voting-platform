package jalau.usersapi.core.domain.entities;

import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;

/**
 * Represents a user in the system.
 * Contains user identifier, name, login and password fields.
 */
@Data
@NoArgsConstructor
@AllArgsConstructor
public class User {
    /** Unique identifier of the user */
    private String id;
    
    /** Name of the user */
    private String name;
    
    /** Login name of the user */
    private String login;
    
    /** Password of the user */
    private String password;
}