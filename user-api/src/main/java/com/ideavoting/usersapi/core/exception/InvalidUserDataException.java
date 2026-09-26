package com.ideavoting.usersapi.core.exception;

public class InvalidUserDataException extends RuntimeException {
	public InvalidUserDataException(String message) {
		super(message);
	}
}
