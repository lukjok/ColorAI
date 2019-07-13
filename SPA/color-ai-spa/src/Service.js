import Configuration from './Configuration';

class ColorService {

  constructor() {
    this.config = new Configuration();
  }

  async predictColor(color) {
    let url = this.config.API_URL;

    return fetch(url, {
      method: 'POST',
      mode: 'cors',
      cache: 'no-cache',
      credentials: 'same-origin',
      headers: {
          'Content-Type': 'application/json'
      },
      redirect: 'follow',
      referrer: 'no-referrer',
      body: JSON.stringify(color)
  })
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