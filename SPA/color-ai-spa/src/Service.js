import Configuration from './Configuration';

class ColorService {

  constructor() {
    this.config = new Configuration();
  }

  async predictColor(color) {
    let url = this.config.API_URL + '/' + color;

    return fetch(url)
      .then(response => {
        if (!response.ok) {
          this.handleResponseError(response);
        }
        return response.json();
      })
      .then(color => {
        console.log("Retrieved items:");
        console.log(color);
        return color;
      })
      .catch(error => {
        this.handleError(error);
      });
  }

  handleResponseError(response) {
      throw new Error("HTTP error, status = " + response.status);
  }

  handleError(error) {
      console.log(error.message);
  }
}
export default ColorService;