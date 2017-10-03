var UploadHub = React.createClass({
    render: function () {
        return (
            <div className="fileuploadContainer">
                <div className="row fileuploadButtonContainer">
                    <input className="btn btn-primary" type="button" value="Browse" />
                    <input className="btn btn-warning" type="button" value="Start Upload" />
                    <input className="btn btn-danger" type="button" value="Cancel" />
                </div>
                <div className="row fileUploadTempFileContainer">
                    <ul>
                        <li className="clearfix">
                            <div className="col-md-6">
                                <label>FileName:  </label> C:\Users\Keith\Pictures\Saved Pictures\sZo700t.jpg
                            </div>
                            <div className="col-md-2">
                                <label>Size:</label> 128kb
                            </div>

                            <div className="col-md-3">
                                <div className='progress '>
                                    <div className='progress-bar progress-bar-success'
                                        role='progressbar'
                                        aria-valuenow='70'
                                        aria-valuemin='0'
                                        aria-valuemax='100'
                                        style={{ width: '70%' }}>
                                        <span className='sr-only'>30% Complete</span>
                                    </div>
                                </div>
                            </div>
                            <div className="col-md-1">
                                <input className="btn btn-danger pull-right" type="button" value="Delete" />
                            </div>
                        </li>
                        <li className="clearfix">
                            <div className="col-md-6">
                                <label>FileName:</label> C:\Users\Keith\Pictures\Saved Pictures\zOrHWVE.jpg
                            </div>
                            <div className="col-md-2">
                                <label>Size: </label> 328kb
                            </div>

                            <div className="col-md-3">
                                <div className='progress '>
                                    <div className='progress-bar progress-bar-success'
                                        role='progressbar'
                                        aria-valuenow='70'
                                        aria-valuemin='0'
                                        aria-valuemax='100'
                                        style={{ width: '70%' }}>
                                        <span className='sr-only'>70% Complete</span>
                                    </div>
                                </div>
                            </div>
                            <div className="col-md-1">
                                <input className="btn btn-danger pull-right" type="button" value="Delete" />
                            </div>
                        </li>
                    </ul>
                </div>

                <div className="row fileUploadFileListcontainer">
                    <table className="table">
                        <thead>
                            <tr>
                                <th>File</th>
                                <th>File Url</th>
                                <th>Size</th>
                                <th>Views</th>
                                <th></th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td>Data.csv</td>
                                <td>http://fileupload.com/data.csv</td>
                                <td>12mb</td>
                                <td>25</td>
                                <td>
                                    <input className="btn btn-warning pull-right" type="button" value="View" />
                                    <input className="btn btn-danger pull-right" type="button" value="Delete" />
                                </td>
                            </tr>
                            <tr className="success">
                                <td>Wages.csv</td>
                                <td>http://fileupload.com/wages.csv</td>
                                <td>500kb</td>
                                <td>900</td>
                                <td>
                                    <input className="btn btn-warning pull-right" type="button" value="View" />
                                    <input className="btn btn-danger pull-right" type="button" value="Delete" />
                                </td>
                            </tr>
                            <tr className="danger">
                                <td>Trucks.csv</td>
                                <td>http://fileupload.com/trucks.csv</td>
                                <td>30kb</td>
                                <td>25</td>
                                <td>
                                    <input className="btn btn-warning pull-right" type="button" value="View" />
                                    <input className="btn btn-danger pull-right" type="button" value="Delete" />
                                </td>
                            </tr>
                            <tr className="info">
                                <td>Cars.csv</td>
                                <td>http://fileupload.com/cars.csv</td>
                                <td>20kb</td>
                                <td>18</td>
                                <td>
                                    <input className="btn btn-warning pull-right" type="button" value="View" />
                                    <input className="btn btn-danger pull-right" type="button" value="Delete" />
                                </td>
                            </tr>
                            <tr className="warning">
                                <td>Data.csv</td>
                                <td>http://fileupload.com/data.csv</td>
                                <td>55kb</td>
                                <td>33</td>
                                <td>
                                    <input className="btn btn-warning pull-right" type="button" value="View" />
                                    <input className="btn btn-danger pull-right" type="button" value="Delete" />
                                </td>
                            </tr>
                            <tr className="active">
                                <td>FoodList.csv</td>
                                <td>http://fileupload.com/Food.csv</td>
                                <td>300kb</td>
                                <td>500</td>
                                <td>
                                    <input className="btn btn-warning pull-right" type="button" value="View" />
                                    <input className="btn btn-danger pull-right" type="button" value="Delete" />
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
        );
    }
});

ReactDOM.render(
    <UploadHub />,
    document.getElementById('content')
);