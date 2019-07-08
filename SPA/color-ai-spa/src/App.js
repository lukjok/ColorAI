import React, {Component } from 'react';
import './App.css';
import { Layout, Menu, Button, Input, Typography } from 'antd';
import { Row, Col } from 'antd';
import ColorService from './Service';

const { Header, Content, Footer } = Layout;
const { Title } = Typography;
const InputGroup = Input.Group;

class App extends Component {

  constructor(props) {
    super(props);
    this.colorService = new ColorService();
    this.colInp = React.createRef();
    this.state = {color: '#fff', loading: false};
  }


  changeColor(code) {
    console.log(code);
    this.setState({loading: true});
    this.colorService.predictColor(code).then(color => {
      this.setState({
        color: color, 
        loading: false
      });
    });
  }

  render() {
    return (
      <Layout style={{height: '100vh'}}>
        <Header style={{ position: 'fixed', zIndex: 1, width: '100%' }}>
          <div className="logo" />
          <Menu
            theme="dark"
            mode="horizontal"
            defaultSelectedKeys={['2']}
            style={{ lineHeight: '64px' }}
          >
            <Menu.Item key="1">nav 1</Menu.Item>
            <Menu.Item key="2">nav 2</Menu.Item>
            <Menu.Item key="3">nav 3</Menu.Item>
          </Menu>
        </Header>
        <Content style={{ marginTop: 64 }}>
          <div ref={ref => this.colScr = ref} style={{ background: this.state.color, textAlign: 'center', height: '100%' }}>
            <div style={{ margin: 'auto', width: '50%' }}>
              <Row>
                <Col span={12} style={{ width: '100%', padding: '30% 0' }}>
                  <InputGroup compact>
                    <Input ref={this.colInp} style={{ width: '70%' }} size='large' placeholder="Enter color name, eg. blue" />
                    <Button 
                    onClick={() => this.changeColor(this.colInp.current.state.value)} 
                    loading={this.state.loading} 
                    style={{ width: '30%' }} 
                    type="primary" 
                    shape="round" 
                    icon="thunderbolt" 
                    theme="filled" 
                    size='large'>
                      Predict
                </Button>
                  </InputGroup>
                </Col>
              </Row>
              <Title level={1}>{this.state.color}</Title>
            </div>
          </div>
        </Content>
      </Layout>
    );
  }
}

export default App;
