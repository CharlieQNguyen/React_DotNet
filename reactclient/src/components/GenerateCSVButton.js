import React, { useState, useRef } from "react";
import { CSVLink } from "react-csv";
import Constants from "../utilities/Constants";

class GenerateCSVButton extends React.Component {
  constructor() {
    super();
    this.state = {
      postData: []
    };
    this.currentRef = React.createRef();
    this.getPostData = this.getPostData.bind(this);
  }

  async getPostData() {
    const url = Constants.API_URL_GET_ALL_POSTS;

    await fetch(url, {
      method: "GET"
    })
      .then(response => response.json())
      .then(postsFromServer => {
        const formattedPostData = this.formattedPostData(postsFromServer);

        this.setState({ postData: formattedPostData }, () => {
          this.currentRef.current.link.click();
        });
      })
      .catch(error => {
        console.log(error);
        alert(error);
      });
  }

  formattedPostData(serverPostData) {
    if (serverPostData.length == 0) {
      return [];
    }
    const resultArray = [];
    const firstArray = Object.keys(serverPostData[0]);
    resultArray.push(firstArray);
    for (let row in serverPostData) {
      const currentRow = serverPostData[row];
      const newRow = [];
      for (let i = 0; i < firstArray.length; i++) {
        newRow.push(currentRow[firstArray[i]]);
      }
      resultArray.push(newRow);
    }
    return resultArray;
  }

  render() {
    return (
      <div>
        <button className="btn btn-secondary btn-lg w-100 mt-4" onClick={this.getPostData}>Download CSV</button>
        <CSVLink
          data={this.state.postData}
          filename="post.csv"
          className="hidden"
          ref={this.currentRef}
          target="_blank"
        />
      </div>
    );
  }
}

export default GenerateCSVButton;
