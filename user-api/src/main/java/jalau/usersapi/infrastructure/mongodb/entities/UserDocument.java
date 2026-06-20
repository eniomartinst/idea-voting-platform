package jalau.usersapi.infrastructure.mongodb.entities;

import org.springframework.data.annotation.Id;
import org.springframework.data.mongodb.core.mapping.Document;
import lombok.Builder;
import lombok.Data;

@Data
@Builder
@Document(collection = "users")
public class UserDocument {
    @Id
    private String id;
    private String name;
    private String login;
    private String password;
}