class LoginReturn {
  String message;
  String metadata;

  LoginReturn({
    required this.message,
    required this.metadata,
  });

  factory LoginReturn.fromMap(Map<String, dynamic> map) {
    return LoginReturn(
      message: map['message'],
      metadata: map['metadata'],
    );
  }
}
