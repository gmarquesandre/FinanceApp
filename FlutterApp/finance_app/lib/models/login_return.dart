class LoginReturn {
  String message;

  LoginReturn({
    required this.message,
  });

  LoginReturn.fromJson(Map<String, dynamic> json) : message = json['message'];
}
