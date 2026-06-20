package jalau.usersapi.core.domain.entities;

import lombok.Data;

@Data
public class Token {
    private String accessToken;
    private String tokenType = "Bearer";
    private String role = "User";
    private User user;
}