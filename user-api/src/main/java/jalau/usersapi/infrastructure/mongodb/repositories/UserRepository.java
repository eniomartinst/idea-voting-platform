package jalau.usersapi.infrastructure.mongodb.repositories;

import jalau.usersapi.core.domain.entities.User;
import jalau.usersapi.core.domain.repositories.IUserRepository;
import jalau.usersapi.infrastructure.mongodb.entities.UserDocument;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.context.annotation.Profile;
import org.springframework.data.mongodb.core.MongoTemplate;
import org.springframework.data.mongodb.core.query.Criteria;
import org.springframework.data.mongodb.core.query.Query;
import org.springframework.stereotype.Repository;

import java.util.List;
import java.util.stream.Collectors;

@Repository
@Profile("mongodb")
public class UserRepository implements IUserRepository {

    @Autowired
    private MongoTemplate mongoTemplate;

    @Override
    public User createUser(User user) {
        UserDocument document = UserDocument.builder()
                .name(user.getName())
                .login(user.getLogin())
                .password(user.getPassword())
                .build();
        UserDocument saved = mongoTemplate.save(document);
        return mapToDomain(saved);
    }

    @Override
    public User updateUser(User user) {
        UserDocument document = UserDocument.builder()
                .id(user.getId())
                .name(user.getName())
                .login(user.getLogin())
                .password(user.getPassword())
                .build();

        mongoTemplate.save(document);
        return user;
    }

    @Override
    public void deleteUser(String id) {
        UserDocument doc = mongoTemplate.findById(id, UserDocument.class);
        if (doc != null) {
            mongoTemplate.remove(doc);
        }
    }

    @Override
    public List<User> getUsers() {
        return mongoTemplate.findAll(UserDocument.class)
                .stream()
                .map(this::mapToDomain)
                .collect(Collectors.toList());
    }

    @Override
    public User getUser(String id) {
        UserDocument doc = mongoTemplate.findById(id, UserDocument.class);
        return doc != null ? mapToDomain(doc) : null;
    }

    @Override
    public User getUserByLogin(String login) {
        Query query = new Query(Criteria.where("login").is(login));
        UserDocument doc = mongoTemplate.findOne(query, UserDocument.class);
        return doc != null ? mapToDomain(doc) : null;
    }

    private User mapToDomain(UserDocument doc) {
        return new User(doc.getId(), doc.getName(), doc.getLogin(), doc.getPassword());
    }
}