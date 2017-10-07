var UploadHub = React.createClass({

    getInitialState: function () {
        return { temporaryFiles: [], allFiles: [], fileUploadHub: $.connection.fileUploadHub };
    },

    fileuploadUpdateProgress: function (id) {
        $("#progress_" + id).prop("aria-valuenow", 100);
        $("#progress_" + id).css("width", "100%");

        setTimeout(function () {
            this.fileuploadDeleteTemp(id);
        }.bind(this), 1000)
    },

    setCookie: function (cname, cvalue, exdays) {
        var d = new Date();
        d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
        var expires = "expires=" + d.toUTCString();
        document.cookie = cname + "=" + cvalue + ";" + expires + ";path=/";
    },

    componentWillMount: function () {
        $.connection.hub.url = "http://localhost:8091/signalr";

        var fileUploadHub = this.state.fileUploadHub;

        this.state.fileUploadHub.client.fileuploadUpdateProgress = this.fileuploadUpdateProgress;
        this.state.fileUploadHub.client.fileuploadRemoveFileFrontList = this.fileuploadRemoveFileFrontList;
        this.state.fileUploadHub.client.fileuploadUpdateRow = this.fileuploadUpdateRow;

        $.connection.hub.start().done(function () {
            console.log(this.state.fileUploadHub.connection.id);
            this.setCookie("ConnectionID", this.state.fileUploadHub.connection.id, 1);
        }.bind(this));

        $.ajax({
            type: "GET",
            url: "/api/File/",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                this.fileuploadUpdateFileList(data);
            }.bind(this)
        });
    },

    fileuploadUploadImages: function (e) {
        var uploadButton = e.target;
        $(uploadButton).prop("disabled", true);

        var files = this.state.temporaryFiles;

        var data = new FormData();

        for (var x = 0; x < files.length; x++) {
            if (files[x].pushed == false) {
                files[x].pushed = true;
                data.append(files[x].id, files[x].fileRef);
                console.log(files[x].id);
            }
        }


        $.ajax({
            type: "POST",
            url: '/api/File',
            contentType: false,
            processData: false,
            data: data,
            success: function (data) {
                this.fileuploadUpdateFileList(data);
                $(uploadButton).removeAttr("disabled");
            }.bind(this),
            error: function (xhr, status, p3, p4) {
                var err = "Error " + " " + status + " " + p3 + " " + p4;
                if (xhr.responseText && xhr.responseText[0] == "{")
                    err = JSON.parse(xhr.responseText).Message;
            }
        });
    },

    fileuploadUpdateFileList: function (filesList) {
        currentFiles = this.state.allFiles;

        for (var i = 0; i < filesList.length; i++) {
            var file = filesList[i];
            currentFiles.push({ id: file.ID, name: file.Filename, url: file.BlobUrl, viewCount: file.Views, size: file.Size });
        }

        this.setState({ allFiles: currentFiles });
    },

    fileuploadUpdateTempList: function () {
        var fileInput = document.getElementById('fileuploadBtnBrowse');
        var filesLoaded = fileInput.files;

        var filesTemp = this.state.temporaryFiles;

        for (var i = 0; i < filesLoaded.length; i++) {
            var file = filesLoaded[i];
            filesTemp.push({ id: this.generateUUID(), name: file.name, size: file.size, fileRef: file, pushed: false });
        }

        this.setState({ temporaryFiles: filesTemp }, this.fileupoadShowHide);
        document.getElementById("fileuploadBtnBrowse").value = "";
    },

    fileuploadCancelTemp: function () {
        this.setState({ temporaryFiles: [] }, this.fileupoadShowHide);
    },

    fileuploadDeleteTemp: function (id) {
        var tempFiles = this.state.temporaryFiles

        for (var i = 0; i < tempFiles.length; i++)
            if (tempFiles[i].id === id) {
                tempFiles.splice(i, 1);
                break;
            }

        this.setState({ temporaryFiles: tempFiles }, this.fileupoadShowHide);
    },

    generateUUID: function generateUUID() {
        var d = new Date().getTime();
        var uuid = 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
            var r = (d + Math.random() * 16) % 16 | 0;
            d = Math.floor(d / 16);
            return (c == 'x' ? r : (r & 0x3 | 0x8)).toString(16);
        });
        return uuid;
    },

    fileupoadShowHide: function () {

        if (this.state.temporaryFiles.length > 0) {
            $('#fileUploadTempFileContainer').show();
        }
        else {
            $('#fileUploadTempFileContainer').hide();
        }
    },

    fileuploadViewUploadedFile: function (id, blobUrl) {
        $.ajax({
            type: "PUT",
            url: '/api/File/' + id,
            contentType: false,
            processData: false,
            success: function (data) {
                
            }.bind(this),
            error: function (xhr, status, p3, p4) {

            }
        });
        console.log(blobUrl);
        window.open(
            blobUrl,
            '_blank' // <- This is what makes it open in a new window.
        );
    },

    fileuploadDeleteUploadedFile: function (id, e) {
        $(e.target).prop("disabled", true);

        $.ajax({
            type: "DELETE",
            url: '/api/File/' + id,
            contentType: false,
            processData: false,
            success: function (data) {

            }.bind(this),
            error: function (xhr, status, p3, p4) {
                $('#fileuploadBtnDelete_' + id).prop("disabled", false);
            }
        });
    },

    fileuploadRemoveFileFrontList: function (id) {
        var allFiles = this.state.allFiles

        for (var i = 0; i < allFiles.length; i++)
            if (allFiles[i].id === id) {
                allFiles.splice(i, 1);
                break;
            }

        this.setState({ allFiles: allFiles }, this.fileupoadShowHide);
    },

    fileuploadUpdateRow: function (data) {
        var allFiles = this.state.allFiles

        for (var i = 0; i < allFiles.length; i++)
            if (allFiles[i].id === data.ID) {
                allFiles[i] = { id: data.ID, name: data.Filename, url: data.BlobUrl, viewCount: data.Views, size: data.Size };
                break;
            }

        this.setState({ allFiles: allFiles }, this.fileupoadShowHide);
    },


    render: function () {
        return (
            <div className="fileuploadContainer">
                <div className="row">
                    <label className="btn btn-primary btn-file fileuploadFunctionButton">
                        Browse <input onChange={this.fileuploadUpdateTempList} id='fileuploadBtnBrowse' type="file" style={{ display: 'none' }} multiple />
                    </label>
                    <input onClick={this.fileuploadUploadImages} className="btn btn-warning fileuploadFunctionButton" type="button" value="Start Upload" />
                    <input onClick={this.fileuploadCancelTemp} className="btn btn-danger fileuploadFunctionButton" type="button" value="Cancel" />
                </div>
                <div id='fileUploadTempFileContainer' className="row fileUploadTempFileContainer">
                    <ul>
                        {this.state.temporaryFiles.map(file =>
                            <li className="clearfix" key={file.id}>
                                <div className="col-md-6">
                                    <label>FileName:  </label> {file.name}
                                </div>
                                <div className="col-md-2">
                                    <label>Size:</label> {file.size}
                                </div>

                                <div className="col-md-3">
                                    <div className='progress '>
                                        <div id={'progress_' + file.id} className='progress-bar progress-bar-success'
                                            role='progressbar'
                                            aria-valuenow='0'
                                            aria-valuemin='0'
                                            aria-valuemax='100'
                                            style={{ width: '0%' }}>
                                            <span className='sr-only'>30% Complete</span>
                                        </div>
                                    </div>
                                </div>
                                <div className="col-md-1">
                                    <input className="btn btn-danger pull-right" onClick={this.fileuploadDeleteTemp.bind(null, file.id)} type="button" value="Delete" />
                                </div>
                            </li>
                        )
                        }
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
                            {this.state.allFiles.map(file =>
                                <tr key={file.id}>

                                    <td>{file.name}</td>
                                    <td>{file.url}</td>
                                    <td>{file.size}</td>
                                    <td>{file.viewCount}</td>
                                    <td>
                                        <input id={'fileuploadBtnDelete_' + file.id} onClick={this.fileuploadDeleteUploadedFile.bind(null, file.id)} className="btn btn-danger pull-right" type="button" value="Delete" />
                                        <input onClick={this.fileuploadViewUploadedFile.bind(null, file.id, file.url)} className="btn btn-warning pull-right fileuploadFunctionButton" type="button" value="View" />
                                    </td>

                                </tr>
                            )
                            }
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