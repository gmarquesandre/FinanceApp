import 'package:financial_app/database/dao/category_dao.dart';
import 'package:financial_app/models/table_models/category.dart';
import 'package:flutter/material.dart';

class CategoryForm extends StatefulWidget {

  final Category? category;

  CategoryForm([this.category]);

  @override
  _CategoryFormState createState() => _CategoryFormState();
}

class _CategoryFormState extends State<CategoryForm> {

  final TextEditingController _nameController = TextEditingController();
  final CategoryDao _dao = CategoryDao();
  final _formKey = GlobalKey<FormState>();


  @override initState(){
    super.initState();
    {

      if(widget.category != null){
        _nameController.text = widget.category!.name;
      }

    }
  }


  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: Form(
        key: _formKey,
        child: Padding(
          padding: const EdgeInsets.fromLTRB(16.0, 0.0, 16.0, 0.0),
          child: GestureDetector(
            onTap: () => FocusScope.of(context).unfocus,
            child: SingleChildScrollView(
              child: Column(
                children: [
                  Padding(
                    padding: const EdgeInsets.only(top: 8.0),
                    child: TextField(
                      controller: _nameController,
                      decoration: InputDecoration(
                        labelText: 'Nome da Categoria',
                      ),
                    ),
                  ),
                  Padding(
                    padding: const EdgeInsets.only(top: 16.0),
                    child: SizedBox(
                      width: double.maxFinite,
                      child: ElevatedButton(
                        onPressed: () {
                          if (_formKey.currentState!.validate()) {

                            final String name = _nameController.text;
                            final Category newCategory = Category(name: name);

                            if(widget.category == null) {

                                _dao.save(newCategory).then((id) =>
                                  Navigator.pop(context, newCategory.toString()));
                            }
                            else{
                              newCategory.id = widget.category!.id;
                              _dao.updateRow(newCategory).then((id) =>
                                  Navigator.pop(context, newCategory.toString()));
                            }
                          }
                          else{
                            }
                          },
                        child: Text('Adicionar'),
                      ),
                    ),
                  )
                ],
              ),
            ),
          ),
        ),
      ),
    );
  }
}
