Ext.define('LoanAgentExt.view.auth.Register', {
    extend: 'Ext.form.Panel',
    xtype: 'registerview',

    title: 'Register',
    bodyPadding: 10,
    width: 300,

    items: [
        {
            xtype: 'textfield',
            name: 'username',
            fieldLabel: 'Username',
            allowBlank: false
        },
        {
            xtype: 'textfield',
            name: 'password',
            fieldLabel: 'Password',
            inputType: 'password',
            allowBlank: false
        },
        {
            xtype: 'textfield',
            name: 'email',
            fieldLabel: 'Email',
            allowBlank: false
        },
        {
            xtype: 'textfield',
            name: 'firstname',
            fieldLabel: 'First Name',
            allowBlank: false
        },
        {
            xtype: 'textfield',
            name: 'lastname',
            fieldLabel: 'Last Name',
            allowBlank: false
        },
        {
            xtype: 'textfield',
            name: 'idnumber',
            fieldLabel: 'ID Number',
            allowBlank: false
        },
        {
            xtype: 'datefield',
            name: 'dateOfBirth',
            fieldLabel: 'Date of Birth',
            allowBlank: false,
            format: 'Y-m-d',
            maxValue: new Date()
        }
    ],

    buttons: [
        {
            text: 'Register',
            formBind: true,
            disabled: true,
            handler: function (btn) {
                const form = btn.up('form').getForm();
                if (form.isValid()) {
                    const values = form.getValues();

                    Ext.Ajax.request({
                        url: LoanAgentExt.app.apiUrl + '/api/users/create',
                        method: 'POST',
                        jsonData: values,
                        success: function (response) {
                            const result = Ext.decode(response.responseText);
                            localStorage.setItem('accessToken', result);

                            Ext.Msg.alert('Success', 'Account created successfully');

                            LoanAgentExt.app.setMainView('LoanAgentExt.view.main.Main');
                        },
                        failure: function () {
                            Ext.Msg.alert('Error', 'Registration failed. Try again.');
                        }
                    });
                }
            }
        }
    ]
});