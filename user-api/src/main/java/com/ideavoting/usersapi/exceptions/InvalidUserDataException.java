package com.ideavoting.usersapi.exceptions;

public class InvalidUserDataException extends RuntimeException {
	public InvalidUserDataException(String message) {
		super(message);
	}
}
