namespace HangFire.Helpers
{
    public static class EmailBody
    {
        public static string EmailStringBody(string email , string emailToken)
        {
            return $@"<html>
                      <head>

                      </head>
                      <body style=""margin: 0; padding: 0; font - family: Arial, Helvetica, sans - serif; "">
                        < div style = ""height: auto;background:linear-gradient(to top,#c9c9ff 50% , #6e6ef6 90%) no-repeat ;width: 400px;padding: 30px;"" >
                          < div >
                            < div >
                              < h1 > Reset Your Password</ h1 >
                              < hr >
                              < p > you are receving this Email because you requested reset password</ p >
                              < p > please tab the button below to choose new password</ p >
                              < a style = ""color: white;border-radius: 4px; display: block;margin: 0 auto;width: 50%;text-align: center;"" href = ""http://localhost:4200/reset?email={email}&code={emailToken}"" ></ a >
                              < p > Kind Regards , < br >< br ></ p >
                            </ div >
                          </ div >
                        </ div >
                      </ body >
                    </ html > ";
        }
    }
}
