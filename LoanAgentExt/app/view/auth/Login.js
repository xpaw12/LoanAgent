Ext.define('LoanAgentExt.view.auth.Login', {
    extend: 'Ext.form.Panel',
    xtype: 'loginview',

    title: 'Login',
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
        }
    ],

    buttons: [
        {
            text: 'Login',
            formBind: true,
            disabled: true,
            handler: function (btn) {
                const form = btn.up('form').getForm();
                if (form.isValid()) {
                    const values = form.getValues();

                    Ext.Ajax.request({
                        url: LoanAgentExt.app.apiUrl + '/api/users/login',
                        method: 'POST',
                        jsonData: values,
                        success: function (response) {
                            const result = Ext.decode(response.responseText);
                            localStorage.setItem('accessToken', result);

                            Ext.Msg.alert('Success', 'Logged in successfully');

                            LoanAgentExt.app.setMainViewByRole(result);
                        },
                        failure: function () {
                            Ext.Msg.alert('Error', 'Login failed. Check your credentials.');
                        }
                    });
                }
            }
        }
    ],

    render: function () {
        this.callParent(arguments);

        const registerLink = Ext.create('Ext.Component', {
            html: '<a href="#" id="registerLink">Don\'t have an account? Register here.</a>',
            margin: '10 0 0 0',
            listeners: {
                afterrender: function () {
                    this.getEl().on('click', function () {
                        LoanAgentExt.app.setMainView('LoanAgentExt.view.auth.Register');
                    });
                }
            }
        });

        this.add(registerLink);
    }
});