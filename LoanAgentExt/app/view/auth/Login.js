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
                    // Send AJAX request to login endpoint
                    Ext.Ajax.request({
                        url: LoanAgentExt.app.apiUrl + '/api/users/login',
                        method: 'POST',
                        jsonData: values,
                        success: function (response) {
                            const result = Ext.decode(response.responseText);
                            // Save JWT token in local storage
                            localStorage.setItem('accessToken', result);

                            Ext.Msg.alert('Success', 'Logged in successfully');

                            // Navigate to main application view
                            //Ext.ComponentManager.get('viewport').removeAll(); // Clear existing components
                            LoanAgentExt.app.setMainView('LoanAgentExt.view.main.Main');
                        },
                        failure: function () {
                            Ext.Msg.alert('Error', 'Login failed. Check your credentials.');
                        }
                    });
                }
            }
        }
    ],

    // Add a register link at the bottom of the form
    render: function () {
        this.callParent(arguments); // Call the parent render method

        // Add a register link
        const registerLink = Ext.create('Ext.Component', {
            html: '<a href="#" id="registerLink">Don\'t have an account? Register here.</a>',
            margin: '10 0 0 0',
            listeners: {
                afterrender: function () {
                    // Add click event to the register link
                    this.getEl().on('click', function () {
                        // Switch to the register view
                        LoanAgentExt.app.setMainView('LoanAgentExt.view.auth.Register'); // Adjust this path based on your folder structure
                    });
                }
            }
        });

        this.add(registerLink); // Add the link to the form
    }
});